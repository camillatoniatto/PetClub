using System;
using System.Collections.Generic;
using System.Text;

namespace PetClub.AppService.ViewModels
{
    public class PagingParametersViewModel
    {
        public PagingParametersViewModel() { }
        public PagingParametersViewModel(int numberPage, int numberRecords)
        {
            NumberPage = numberPage;
            NumberRecords = numberRecords;
        }

        public int NumberPage { get; set; }
        public int NumberRecords { get; set; }
    }
}
