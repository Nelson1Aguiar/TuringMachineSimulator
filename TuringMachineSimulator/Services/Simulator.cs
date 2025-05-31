using System.Reflection.PortableExecutable;
using TuringMachineSimulator.Enum;

public class Simulator
{
    private readonly TuringMachine _machine;
    private string _wordToInit;
    private long _stepLimit;
    private int _headPosition;
    private char _blankSymbol;
    private string _currentState;
    private long _currentStep;
    private bool _continueSimulation;
    private List<char> _wordComplete;
    private List<string> _configurations = new List<string>();

    public Simulator(TuringMachine machine, long stepLimit = 100)
    {
        _machine = machine;
        _wordToInit = machine.Word;
        _stepLimit = machine.MaxSteps.Value;
        _blankSymbol = machine.BlankSymbol.FirstOrDefault();
        _headPosition = 0;
        _currentStep = 0;
        _continueSimulation = true;
    }

    public (bool?, string) InitSimulation()
    {
        try
        {
            bool? acceptWord = false;
            string stopState = string.Empty;

            _wordComplete = new List<char> { _machine.InitialSymbol!.FirstOrDefault() };
            _wordComplete.AddRange(_wordToInit.ToCharArray());
            _wordComplete.Add(_blankSymbol);

            _headPosition = 1;
            _currentState = _machine.InitialState;

            _configurations.Add($"Passo {_currentStep}: Estado={_currentState}, Posição={_headPosition}, Lendo={_wordComplete[_headPosition]}, Palavra={new string(_wordComplete.ToArray())}");

            while (_continueSimulation)
            {
                if (!ValidateSymbol(_wordComplete[_headPosition]))
                    throw new ArgumentException("A palavra contém símbolos inválidos para a máquina de Turing.");

                string key = $"{_currentState},{_wordComplete[_headPosition]}";

                ExecuteTransition(key);
            }

            acceptWord = string.Equals(_currentState, _machine.AcceptState);
            acceptWord = !string.Equals(_currentState, _machine.AcceptState) && !string.Equals(_currentState, _machine.RejectState) ? null : acceptWord;

            stopState = $"Passo {_currentStep}: Estado={_currentState}, Posição={_headPosition}, Lendo={_wordComplete[_headPosition]}, Palavra={new string(_wordComplete.ToArray())}";

            return (acceptWord, stopState);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void ExecuteTransition(string key)
    {
        try
        {
            if (!VerifyContinueSimulation() || string.Equals(_currentState, _machine.AcceptState) || string.Equals(_currentState, _machine.RejectState))
            {
                _continueSimulation = false;
                return;
            }

            if (_machine.Transitions.Keys.Contains(key))
            {
                Transition transition = _machine.Transitions[key];

                if (!string.IsNullOrWhiteSpace(transition.NextState))
                {
                    _currentState = transition.NextState;
                }

                if (!string.IsNullOrWhiteSpace(transition.WriteSymbol))
                {
                    _wordComplete[_headPosition] = transition.WriteSymbol.FirstOrDefault();
                }

                if (!string.IsNullOrWhiteSpace(transition.MoveDirection))
                {
                    Movements movement = (Movements)transition.MoveDirection.FirstOrDefault();

                    switch (movement)
                    {
                        case Movements.Left:
                            _headPosition--;
                            if (_headPosition < 0)
                                _headPosition = 0;
                            break;
                        case Movements.Right:
                            _headPosition++;
                            if (_headPosition >= _wordComplete.Count)
                                _wordComplete.Add(_blankSymbol);
                            break;
                        default:
                            break;
                    }
                }

                _currentStep++;

                _configurations.Add($"Passo {_currentStep}: Estado={_currentState}, Posição={_headPosition}, Lendo={_wordComplete[_headPosition]}, Palavra={new string(_wordComplete.ToArray())}");

                return;
            }

            _currentState = _machine.RejectState;
            _continueSimulation = false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private bool VerifyContinueSimulation()
    {
        if (_currentStep >= _stepLimit)
        {
            Console.WriteLine("Limite de passos atingido, deseja continuar? \n\r1 - Sim\n\r2 - Não");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if (input == 2)
                    return false;

                Console.WriteLine("Digite quantas a mais devem ocorrer: ");

                if (long.TryParse(Console.ReadLine(), out long newStepLimit))
                {
                    _stepLimit += newStepLimit;
                    return true;
                }

                else
                {
                    Console.WriteLine("Entrada inválida. O limite de passos não foi alterado.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida.");
                return false;
            }
        }

        return true;
    }

    private bool ValidateSymbol(char symbol)
    {
        if (_machine.TapeAlphabet is not null && _machine.TapeAlphabet.Any())
            return _machine.TapeAlphabet.Contains(char.ToString(symbol));

        return _machine.InputAlphabet.Contains(char.ToString(symbol)) 
                                || Equals(symbol, _blankSymbol)
                                || Equals(symbol, _machine.InitialSymbol!.FirstOrDefault());
    }

    public void PrintComputations()
    {
        Console.Clear();

        foreach (string configuration in _configurations)
        {
            Console.WriteLine(configuration);
            Console.WriteLine("--------------------------------------------------");
        }

        Console.ReadLine();
    }
}