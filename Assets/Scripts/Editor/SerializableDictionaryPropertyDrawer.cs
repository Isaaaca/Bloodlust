using UnityEditor;

[CustomPropertyDrawer(typeof(GameEventDictionary))]
[CustomPropertyDrawer(typeof(SequenceDictionary))]
public class AnySerializableDictionaryPropertyDrawer :
SerializableDictionaryPropertyDrawer
{ }