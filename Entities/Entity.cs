namespace CoreBase.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}", Id);
        }
    }

    public abstract class EntityWithSoftDelete : Entity, ISoftDeletable
    {
        public bool Deleted { get; set; }
    }

    public interface ISoftDeletable
    {
        bool Deleted { get; set; }
    }
}
