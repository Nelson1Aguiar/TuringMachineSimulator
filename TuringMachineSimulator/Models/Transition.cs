using System.Text.Json.Serialization;

public class Transition
{
    [JsonPropertyName("next_state")]
    public string NextState { get; set; }

    [JsonPropertyName("write")]
    public string WriteSymbol { get; set; }

    [JsonPropertyName("move")]
    public string? MoveDirection { get; set; }
}