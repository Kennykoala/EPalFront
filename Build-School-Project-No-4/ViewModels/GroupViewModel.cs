using Build_School_Project_No_4.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class GroupViewModel
    {

        //public IEnumerable<MemberViewModel> MeetLikes { get; set; }
        //public IEnumerable<ProductViewModel> EPalIndex { get; set; }
        //public ProductViewModel ProductCards { get; set; }
        //public CategoryViewModel GamesDetails{ get; set; }
        public IEnumerable<ProductViewModel> EPalIndex { get; set; }
        //public IEnumerable<ChillMeetViewModel> ChillMeetResult { get; set; }
        //public IEnumerable<MemberViewModel> Matches { get; set; }

        public IEnumerable<WalletViewModel> wallets { get; set; }
        public AddgameViewModel addgame { get; set; }
        //public ProductPlanSet plansetGV { get; set; }
        //public D FollowingMember { get; set; }
        //public IEnumerable<FollowViewModel> FollowerMember { get; set; }
        //public IEnumerable<ProfileViewModel> Profiles { get; set; }
        //public MemberViewModel MemberData { get; set; }

        //public MemberRegisterViewModel MemberRegister { get; set; }
        //public MemberLoginViewModel MemberLogin { get; set; }
        //public MemberInfoViewModel MemberInfo { get; set; }

        //public OrderViewModel Order { get; set; }

        public DetailViewModel Deets { get; set; }
        public AddToCartViewModel AddCart { get; set; }
        public CheckoutViewModel Checkout { get; set; }
        public OrderViewModel OrderDetails { get; set; }
        public OrderConfirmationViewModel OrderConfirmDetails { get; set; }
    }

}