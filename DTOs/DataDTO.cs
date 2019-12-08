using System;
using System.Collections.Generic;

namespace CoreBase.DTOs
{
    public class DataDTO<T>
    {
        private IList<T> _data;
        public virtual IList<T> Data
        {
            get { return _data; }
            set { _data = value; Total = (value != null) ? value.Count : 0; }
        }

        public virtual int Total { get; set; }

        public DataDTO(IList<T> List)
        {
            if (List == null) throw new ArgumentNullException("List");
            Data = List;
            Total = List.Count;
        }

    }
}
