using UnityEditor;

[CustomPropertyDrawer(typeof(EventSequenceDictionary))]
public class AnySerializableDictionaryPropertyDrawer :
SerializableDictionaryPropertyDrawer
{ }