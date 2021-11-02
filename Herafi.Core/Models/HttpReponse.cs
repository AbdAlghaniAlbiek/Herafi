using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;

namespace Herafi.Core.Models
{
    public class HttpReponse : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Response _response;
        private string _errorMessage;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        [JsonProperty(PropertyName ="response")]
        public Response Response
        {
            get { return _response; }
            set { _response = value; OnPropertyChanged(); }
        }



        [JsonProperty(PropertyName ="errorMessage")]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
        }


    }

    public class Response : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private object _result;
        private string _token;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        [JsonProperty(PropertyName ="result")]
        public object Result
        {
            get { return _result; }
            set { _result = value; OnPropertyChanged(); }
        }


        [JsonProperty(PropertyName ="token")]
        public string Token
        {
            get { return _token; }
            set { _token = value; OnPropertyChanged(); }
        }

    }

}
