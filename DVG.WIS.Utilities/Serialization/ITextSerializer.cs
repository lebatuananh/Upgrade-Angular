using System.IO;

namespace DVG.WIS.Utilities.Serialization
{
    public interface ITextSerializer
    {
        void Serialize<T>(TextWriter writer, T objectGraph);
        T Deserialize<T>(TextReader reader);
    }
}
