using System.Collections.Generic;

namespace ProductInventory.Model
{
    public class LookupItem
    {
        public int Id { get; set; }
        public string DisplayProduct { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as LookupItem;
            return item != null &&
                   Id == item.Id &&
                   DisplayProduct == item.DisplayProduct;
        }

        public override int GetHashCode()
        {
            var hashCode = -188999460;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayProduct);
            return hashCode;
        }

        public override string ToString()
        {
            string outString = Id + ". " + DisplayProduct;

            return outString;
        }
    }
}
