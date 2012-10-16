using System;
using System.Collections;

namespace LSIDClient
{
	public interface Enumeration
    {
        Boolean hasMoreElements();
        Object nextElement();

	}

    public class IColEnumeration : Enumeration
    {
        ArrayList m_Items = new ArrayList();
        int m_Index = 0;

        public IColEnumeration()
        {
        }

        public IColEnumeration(ICollection col)
        {
            IEnumerator e = col.GetEnumerator();
            while (e.MoveNext())
            {
                m_Items.Add(e.Current);
            }
        }

        public void Add(Object element)
        {
            m_Items.Add(element);
        }

        #region Enumeration Members

        public Boolean hasMoreElements()
        {
            return m_Items.Count > m_Index;
        }

        public Object nextElement()
        {
            Object item = m_Items[m_Index];
            m_Index++;
            return item;
        }

        #endregion
    }
}
