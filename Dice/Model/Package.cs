using System.Xml.Serialization;

namespace Dice.Model
{
    /// <summary>
    ///     NuGet Package
    /// </summary>
    [XmlRoot(ElementName = "package")]
    public class Package
    {
        /// <summary>
        ///     Id (name) of NuGet package
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        /// <summary>
        ///     Version of NuGet package
        /// </summary>
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        /// <summary>
        ///     TargetFramework of NuGet package
        /// </summary>
        [XmlAttribute(AttributeName = "targetFramework")]
        public string TargetFramework { get; set; }
    }
}