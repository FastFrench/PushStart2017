using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Common
{
    interface IMetroLocation
    {
        Station Station { get; set; }
        Line Line { get; set; }
    }
}
