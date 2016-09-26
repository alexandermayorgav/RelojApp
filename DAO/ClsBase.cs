using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    [Serializable]
    public class ClsBase
    {
        protected String strError;
	    protected String strSysError;
	    protected Boolean bError;

	    public Boolean setError(String msg,String strSysError)
	    {
		    this.strError=msg;
		    this.strSysError=strSysError;
		    this.bError=true;
		    return false;
	    }

	

	    public Boolean getError()
	    {
		    return this.bError;
	    }

	    public String getStrError()
	    {
		    return this.strError;
	    }

	    public String getStrSysError()
	    {
		    return this.strSysError;
	    }
        
            

	    
    }
}
