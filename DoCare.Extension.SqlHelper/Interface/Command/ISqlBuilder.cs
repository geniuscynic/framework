using System.Text;

namespace DoCare.Extension.SqlHelper.Interface.Command
{
    interface ISqlBuilder
    {
        StringBuilder Build(bool ignorePrefix = true);
    }
}
