namespace Tool.Compet.Json;

using System.Text.Json;
using System.Text.Json.Serialization;

/// Json helper for converter Json vs Object.
/// See https://www.jacksondunstan.com/articles/3303 to choose best approach on each serialize/deserialize.
/// For eg,.
/// - Serialize an object to json => Lit is optimal
/// - Deserialize small string => Unity is optimal
/// - Deserialize large string => Newton is optimal
public class DkJsons {
	private static readonly JsonSerializerOptions Option_Indent = new() {
		WriteIndented = true,
	};
	private static readonly JsonSerializerOptions Option_Indent_Ignore = new() {
		WriteIndented = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.Never
	};
	private static readonly JsonSerializerOptions Option_NoIndent = new() {
		WriteIndented = false
	};
	private static readonly JsonSerializerOptions Option_NoIndent_Ignore = new() {
		WriteIndented = false,
		DefaultIgnoreCondition = JsonIgnoreCondition.Never
	};

	/// <summary>
	/// Convert obj to json string.
	/// Each field/properties in the object should be annotated with [JsonPropertyName()] attribute
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="indent">Also write indent or not. Default is false to compact size (minification optimized)</param>
	/// <param name="ignore">Skip Serialize/Deserialize ignored fields or not. Default is true to skip ignored fields.</param>
	/// <returns></returns>
	public static string ToJson(object obj, bool indent = false, bool ignore = true) {
		var options = indent ?
			(ignore ? Option_Indent_Ignore : Option_Indent) :
			(ignore ? Option_NoIndent_Ignore : Option_NoIndent);

		return JsonSerializer.Serialize(obj, options: options);
	}

	public static string ToJson(object obj, JsonSerializerOptions options) {
		return JsonSerializer.Serialize(obj, options: options);
	}

	/// <summary>
	/// Convert nullable obj to json string.
	/// Each field/properties in the object should be annotated with [JsonPropertyName()] attribute
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="indent">Also write indent or not. Default is false to compact size (minification optimized)</param>
	/// <param name="ignore">Skip Serialize/Deserialize ignored fields or not. Default is true to skip ignored fields.</param>
	/// <returns></returns>
	public static string? ToJsonOrNull(object? obj, bool indent = false, bool ignore = true) {
		var options = indent ?
			(ignore ? Option_Indent_Ignore : Option_Indent) :
			(ignore ? Option_NoIndent_Ignore : Option_NoIndent);

		return obj is null ? null : JsonSerializer.Serialize(obj, options: options);
	}

	public static string? ToJsonOrNull(object? obj, JsonSerializerOptions options) {
		return obj is null ? null : JsonSerializer.Serialize(obj, options: options);
	}

	/// <summary>
	/// Convert json string to obj without try/catch.
	/// Each field/properties in the object should be annotated with [JsonPropertyName()] attribute
	/// TechNote: Add `where T : class` to its function to allow return nullable value.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="json"></param>
	/// <returns></returns>
	public static T? ToObj<T>(string? json) where T : class {
		return json is null ? null : JsonSerializer.Deserialize<T>(json);
	}

	public static T? ToObjWith<T>(string? json, JsonSerializerOptions options) where T : class {
		return json is null ? null : JsonSerializer.Deserialize<T>(json, options: options);
	}

	/// <summary>
	/// Convert given JsonElement to given type without try/catch.
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

	public static T? ToObjFromJsonElement<T>(object element, JsonSerializerOptions options) where T : class {
		if (element is JsonElement jsonElement) {
			return jsonElement.Deserialize<T>(options: options);
		}
		return null;
	}
}
