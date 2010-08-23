using System;
using System.Collections.Generic;
using System.Text;


namespace TapirDotNET
{
    public class TpServerClass 
    {

        public static ITapirServer CreateClass(string asmClassName)
        {
            ITapirServer cls = null;

            try
            {

                string asmName = null;
                string className = "";
                string[] bits = asmClassName.Split(',');
                if (bits.Length == 1)
                {
                    className = bits[0];
                }
                else if (bits.Length == 2)
                {
                    asmName = bits[0];
                    className = bits[1];
                }

                System.Runtime.Remoting.ObjectHandle o = Activator.CreateInstance(asmName, className);
                cls = (ITapirServer)o.Unwrap();
            }
            catch (Exception ex)
            {
                new TpDiagnostics().Append(TpConfigManager.CFG_INTERNAL_ERROR, "Error loading server class (" + asmClassName + ") : " + ex.Message, TpConfigManager.DIAG_ERROR);
            }

            return cls;
        }
    }
}

