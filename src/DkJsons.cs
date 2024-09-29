namespace Tool.Compet.Json;

using System.Text.Json;

/// Json helper for converter Json vs Object.
/// See https://www.jacksondunstan.com/articles/3303 to choose best approach on each serialize/deserialize.
/// For eg,.
/// - Serialize an object to json => Lit is optimal
/// - Deserialize small string => Unity is optimal
/// - Deserialize large string => Newton is optimal
public class DkJsons {
	private static readonly JsonSerializerOptions WriteIndentedOpt = new() { WriteIndented = true };
	private static readonly JsonSerializerOptions WriteNotIndentedOpt = new() { WriteIndented = false };

	/// <summary>
	/// Convert obj to json string.
	/// Each field/properties in the object should be annotated with [JsonPropertyName()] attribute
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="writeIndented"></param>
	/// <returns></returns>
	public static string ToJson(object obj, bool writeIndented = false) {
		return JsonSerializer.Serialize(obj, options: writeIndented ? WriteIndentedOpt : WriteNotIndentedOpt);
	}

	/// <summary>
	/// Convert json string to obj.
	/// Each field/properties in the object should be annotated with [JsonPropertyName()] attribute
	/// TechNote: Add `where T : class` to its function to allow return nullable value.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="json"></param>
	/// <returns></returns>
	public static T? ToObj<T>(string? json) where T : class {
		return json is null ? null : JsonSerializer.Deserialize<T>(json);
	}

	/// <summary>
	/// Convert given JsonElement to given type.
	/// JsonElement is created when dotnet parse payload to object.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="element">Should be JsonElement. Otherwise, null be returned.</param>
	/// <returns></returns>
	public static T? ToObjFromJsonElement<T>(object element) where T : class {
		if (element is JsonElement jsonElement) {
			return jsonElement.Deserialize<T>();
		}
		return null;
	}
}
