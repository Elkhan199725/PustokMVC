namespace PustokMVC.Models
{
    /// <summary>
    /// Serves as the base class for entity models, providing common properties.
    /// </summary>
    /// <remarks>
    /// The BaseEntity class is designed to ensure that all entity models within the PustokMVC application
    /// have a consistent set of properties related to identity, status, and timestamps. This approach aids
    /// in standardizing model structures, facilitating easier data management and auditing. Key properties include:
    /// 
    /// - Id: A unique identifier for entities, used as the primary key in the database.
    /// - IsActive: A flag indicating whether the entity is active or inactive, useful for soft delete operations or hiding entities without permanently deleting them.
    /// - CreatedDate: The timestamp when the entity was created, automatically set to the current UTC time.
    /// - ModifiedDate: The optional timestamp of the last modification to the entity, allowing for tracking changes over time.
    /// 
    /// This base class model supports not only data integrity and consistency but also simplifies the implementation
    /// of common behaviors across different entity types within the application.
    /// </remarks>
    public class BaseEntity
    {
        /// <summary>
        /// Gets or sets the primary key for entities.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Indicates whether the entity is considered active.
        /// </summary>
        /// <remarks>
        /// This property is especially useful for soft delete functionality and controlling visibility of entities without removing them from the database.
        /// Defaults to true.
        /// </remarks>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// The date and time when the entity was created.
        /// </summary>
        /// <remarks>
        /// Automatically set to the current UTC date and time when the entity instance is created.
        /// </remarks>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date and time when the entity was last modified.
        /// </summary>
        /// <remarks>
        /// Optional. Can be used to track the latest modification time of the entity. If not set, it remains null.
        /// </remarks>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Sets the CreatedDate and ModifiedDate to the current UTC datetime.
        /// </summary>
        public void SetCreationAndModificationDate()
        {
            DateTime now = DateTime.UtcNow;
            // If CreatedDate is default, it means the entity is newly created.
            if (CreatedDate == default)
            {
                CreatedDate = now;
            }
            ModifiedDate = now;
        }
    }
}
