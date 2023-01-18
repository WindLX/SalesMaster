using System;

namespace SalesMaster.Utils
{
    public interface IChangeState
    {
        event Action<object, string> OnChangeState;
    }

    public interface IChangeLineNumber
    {
        event Action<object, string> OnChangeLineNumber;
    }

    public interface IChangeChosenSalesList
    {
        event Action<object, string> OnChangeChosenSalesList;
    }
}
