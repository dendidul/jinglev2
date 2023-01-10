using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.General.Enum
{
    public enum EnumCollection
    {

    }

    public enum Registration
    {
        Submit = 1,
        InProgress = 2,
        Completed = 3,
        Rejected = -1

    }
    public enum Status
    {
        Active = 1,
        NonActive = 0

    }
    public enum PayMethod
    {
        TransferManual = 1,
        VirtualAccount = 2,
        Gopay = 3

    }

    public enum BookingFlow
    {
        WaitingPayment = 0,
        Submit = 1,
        Paid = 2,
        ProjectAccepted = 3,
        RecordingProcess = 4,
        MaterialAccepted = 5,
        ProjectCompleted = 6,
        RateTalent = 7,
        ReactionVideo = 8,
        Expired = -1,
        Refund = -2,
        RefundSubmitted = -5,
        RefundCompleted = -8

    }

    public enum CheckingBook
    {
        CheckingUnCompleted = 11,
        CheckingOutOfDeadlineInProgress = 12,
        CheckingExpiredPaymentTime = 13,
        CheckingCompletedBooking = 14,

        CheckingForNotifUserExpiredPayment = 21,
        CheckingForNotifUserWillExpirePayment = 22,
        CheckingForNotifUserBookingWillRefund = 23,
        CheckingForNotifTalentBookingNearOfDeadline = 24,
        CheckingForNotifTalentBookingOutOfDeadline = 25,
        CheckingExpiredForSubmitStatus = 30
       
    }

    public enum FileCategory
    {
        Photo = 0,
        Video = 1
    }

    public enum EmailTargetType
    {
        User = 1,
        Talent = 2
    }

    public enum Role
    {
        Admin = 1,
        User = 2,
        Talent = 3,
        Agency = 9
    }

    public enum TalentCat
    {
        New = 52
    }

    public enum TopupStatus
    {
        Rejected = -1,
        New = 0,
        Request = 1,
        Topup = 2
    }


    //public enum RequestStatus
    //{
    //    Submit = 0,
    //    InProgress = 1,
    //    Completed = 2,
    //    Cancelled = -1
    //}


}
