using System.Collections.Generic;
using System.Xml.Serialization;

namespace Dice.Model
{
    /// <summary>
    ///     NuGet Packages from packages.config
    /// </summary>
    [XmlRoot(ElementName = "packages")]
    public class Packages
    {
        /// <summary>
        ///     List of package entries
        /// </summary>
        [XmlElement(ElementName = "package")]
        public List<Package> List { get; set; }
    }
}