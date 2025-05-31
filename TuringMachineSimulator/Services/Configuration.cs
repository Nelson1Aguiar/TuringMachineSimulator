using System.Text.Json;
using TuringMachineSimulator.Interfaces;

public class Configuration : IConfiguration
{
    public Simulator InitMachine(string jsonMachine)
    {
        TuringMachine machine = JsonSerializer.Deserialize<TuringMachine>(jsonMachine) ??
                                        throw new ArgumentNullException("Máquina inválida");

        machine.InitialSymbol ??= "⊢";
        machine.BlankSymbol ??= "⊔";
        machine.MaxSteps ??= 100;
        
        return new Simulator(machine);
    }
}