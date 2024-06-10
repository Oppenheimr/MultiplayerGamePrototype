
using PlayFab;

namespace Network.PlayFab.Models
{
    public class ServerResultItem<T>
    {
        public bool success;
        public string error;
        public string generatedError;
        public T item;
        
        public ServerResultItem()
        {
            success = true;
            error = string.Empty;
            generatedError = string.Empty;
        }
        
        public ServerResultItem(PlayFabError playFabError)
        {
            success = false;
            
            // ErrorDetails listesinin ilk elemanını almak için
            if (playFabError.ErrorDetails != null && playFabError.ErrorDetails.Values.Count > 0)
            {
                // İlk Value List<string> almak için
                var firstErrorDetailList = playFabError.ErrorDetails.Values.GetEnumerator();
                firstErrorDetailList.MoveNext();
                var firstErrorDetail = firstErrorDetailList.Current;

                if (firstErrorDetail != null && firstErrorDetail.Count > 0)
                {
                    error = firstErrorDetail[0];
                }
                else
                {
                    error = "No error details available";
                }
            }
            else
            {
                error = "No error details available";
            }
            
            generatedError = playFabError.GenerateErrorReport();
        }
    }
}