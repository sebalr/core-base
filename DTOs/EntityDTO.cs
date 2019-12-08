namespace CoreBase.DTOs
{
    public abstract class EntityDTO
    {
        public int Id { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}", Id);
        }

        public void validate() { }
    }
}
