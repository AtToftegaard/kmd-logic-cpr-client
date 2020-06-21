// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Kmd.Logic.Cpr.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CprSubscription
    {
        /// <summary>
        /// Initializes a new instance of the CprSubscription class.
        /// </summary>
        public CprSubscription()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CprSubscription class.
        /// </summary>
        public CprSubscription(System.Guid? id = default(System.Guid?), System.Guid? configurationId = default(System.Guid?), System.Guid? subscriptionId = default(System.Guid?), System.Guid? cprPersonId = default(System.Guid?))
        {
            Id = id;
            ConfigurationId = configurationId;
            SubscriptionId = subscriptionId;
            CprPersonId = cprPersonId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public System.Guid? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "configurationId")]
        public System.Guid? ConfigurationId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "subscriptionId")]
        public System.Guid? SubscriptionId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "cprPersonId")]
        public System.Guid? CprPersonId { get; set; }

    }
}