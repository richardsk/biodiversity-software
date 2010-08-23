namespace TapirDotNET 
{

	public class XsComplexType:XsType
	{
		private Utility.OrderedMap mDeclaredAttributeUses = new Utility.OrderedMap();
		private object mrContentType;
        private bool mIsMixed = false;
		private bool mSimpleTypeDerivation = false;
		
		public XsComplexType(string name, string targetNamespace, string defaultNamespace, bool isGlobal, bool isMixed)
            : base(name, targetNamespace, defaultNamespace, isGlobal)
		{
            this.mIsMixed = isMixed;
			this.mIsSimple = false;
		}
		
		
		public virtual void  AddDeclaredAttributeUse(object xsAttributeUse)
		{
			this.mDeclaredAttributeUses.Push(xsAttributeUse);
		}// end of member function AddDeclaredAttributeUse
		
		public virtual void  AddContentType(object rContentType)
		{
			this.mrContentType = rContentType;
		}// end of member function AddContentType
		
		public virtual Utility.OrderedMap GetDeclaredAttributeUses()
		{
			return this.mDeclaredAttributeUses;
		}// end of member function GetDeclaredAttributeUses
		
		public virtual object GetContentType()
		{
			return this.mrContentType;
		}// end of member function GetContentType
		
		public void SetSimpleTypeDerivation(bool val)
		{
			this.mSimpleTypeDerivation = val;
		}

		public bool HasSimpleContent()
		{
			return this.mSimpleTypeDerivation;
		}

        public bool IsMixed()
        {
            return mIsMixed;
        }

		 /**
		* Internal method called before serialization
		*
		* @return array Properties that should be considered during serialization
		*/
		public override Utility.OrderedMap __sleep()
		{
			return Utility.OrderedMap.Merge(base.__sleep(), new Utility.OrderedMap("mDeclaredAttributeUses", "mrContentType"));
		}// end of member function __sleep
	}
}
