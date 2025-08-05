using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ButterflyCinema.Models.ViewModels
{
    public class ComboItemViewModel
    {
        public string ConcessionId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateComboViewModel
    {
        [Required(ErrorMessage = "Mã combo là bắt buộc.")]
        public string ComboId { get; set; }

        [Required(ErrorMessage = "Tên combo là bắt buộc.")]
        public string ComboName { get; set; }

        [Required(ErrorMessage = "Giá bán là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán phải là số dương.")]
        public int ComboPrice { get; set; }

        public string ComboStatus { get; set; }

        public List<ComboItemViewModel> ComboItems { get; set; }

        public CreateComboViewModel()
        {
            ComboItems = new List<ComboItemViewModel>
            {
                new ComboItemViewModel(),
                new ComboItemViewModel(),
                new ComboItemViewModel()
            };
        }
    }
}
