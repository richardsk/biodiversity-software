using System;
using System.Collections.Generic;
using System.Text;

public interface IXsTypedObject
{
    string  GetTargetNamespace(); 
    void    SetType( object simpleType );
    object  GetType();
	bool    HasDefaultValue();
	object  GetDefaultValue();
	bool    HasFixedValue();
    object  GetFixedValue();
}

