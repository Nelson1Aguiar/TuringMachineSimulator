using System.Text.Json.Serialization;

public class TuringMachine
{
    [JsonPropertyName("states")]
    public List<string> States { get; set; }

    [JsonPropertyName("input_alphabet")]
    public List<string> InputAlphabet { get; set; }

    [JsonPropertyName("tape_alphabet")]
    public List<string>? TapeAlphabet { get; set; }

    [JsonPropertyName("blank")]
    public string BlankSymbol { get; set; }

    [JsonPropertyName("initial_symbol")]
    public string? InitialSymbol { get; set; }

    [JsonPropertyName("initial_state")]
    public string InitialState { get; set; }

    [JsonPropertyName("accept_state")]
    public string AcceptState { get; set; }

    [JsonPropertyName("reject_state")]
    public string RejectState { get; set; }

    [JsonPropertyName("word")]
    public string Word { get; set; }

    [JsonPropertyName("max_steps")]
    public long? MaxSteps { get; set; }

    [JsonPropertyName("transitions")]
    public Dictionary<string, Transition> Transitions { get; set; }

}