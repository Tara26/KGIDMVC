using KGID_Models.KGID_User;
using System.Collections.Generic;
using System;
using DLL.DDOMasterDLL;

namespace BLL.DDOMasterBLL
{
    public class DDOMasterBLL: IDDOMasterBLL
    {  
            private readonly IDDOMasterDLL _IDDOMasterdll;
            public DDOMasterBLL()
            {
            this._IDDOMasterdll = new DDOMasterDLL();
            }
            public IEnumerable<tbl_ddo_master> DDOMasterbll()
            {
                var result = _IDDOMasterdll.DDOMasterdll();
                return result;
            }      
        }
    }

