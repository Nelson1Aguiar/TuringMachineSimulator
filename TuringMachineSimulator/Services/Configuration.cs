using System.Text.Json;
using TuringMachineSimulator.Interfaces;

public class Configuration : IConfiguration
{
    public Simulator InitMachine(string jsonMachine)
    {
        TuringMachine machine = JsonSerializer.Deserialize<TuringMachine>(jsonMachine) ??
                                        throw new ArgumentNullException("Máquina inválida");

        machine.InitialSymbol ??= "⊢";

        if (machine.MaxSteps != null && machine.MaxSteps > 0)
            return new Simulator(machine, machine.MaxSteps.Value);
        
        return new Simulator(machine);
    }
}