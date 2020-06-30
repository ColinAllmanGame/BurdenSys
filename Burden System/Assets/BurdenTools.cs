using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoStudios.Burdens
{
    public delegate BurdenClone BurdenProcess(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver);


    public static class BurdenTools
    {
        public static int GetNumInstancesHeld(Burden b,BurdenInventory i)
        {
            Debug.LogError("shit aint done");
            return 0;
        }

        public static List<BurdenInventory> GetOwnersOfBurdenType(Burden b)
        {
            Debug.LogError("shit aint done");
            return null;
        }



        public enum BurdenReceiverType
        {
            DEFAULT,
            //alphabetical. may deviate from alphabet, main characters should not change position
            AlkyoneReceiver,
            GloryReceiver,
            M21Receiver,
            RaviReceiver,
            SlaughterReceiver,
        }

        public enum BurdenSenderType
        {
            DEFAULT,
            //alphabetical. may deviate from alphabet, main characters should not change position
            AlkyoneSender,
            GlorySender,
            M21Sender,
            RaviSender,
            SlaughterSender,
        }
    

        public static bool TransferBurden(BurdenClone burden,CharacterBurdenManager sender, CharacterBurdenManager receiver)
        {

            //burden.BurdenPreSend(sender); //burden prepares for dispatch while inside first container. 
            ////Undoing changes in the current container applied on arrival, removing barks, etc.
            //
            //Strongly consider verifying the ability for sender and receiver to "pre-approve" the transfer. As it is very hard to undo.



            bool SendSuccess = sender.burdenInventory.DispatchBurden(burden,sender.burdenInventory,receiver.burdenInventory);
            bool ReceiveSuccess = false;
            if (SendSuccess)
            {
                ReceiveSuccess = receiver.TryAddBurden(burden, sender, receiver);
            }
            else
            {
                Debug.LogError("burden transfer failed, sender failure.");
                return false;
            }
            
            if(ReceiveSuccess)
            {
                return true;
            }
            else
            {
                Debug.LogWarning("burden transfer failed, receiver failure.");
                return false;
            }
        }


        public static BurdenProcess GetBurdenProcessReceiver(BurdenReceiverType senderType)
        {
            switch (senderType)
            {
                case BurdenReceiverType.DEFAULT:
                    return DEFAULTBurdenReceiver;
                case BurdenReceiverType.AlkyoneReceiver:
                    return AlkyoneBurdenReceiver;
                case BurdenReceiverType.GloryReceiver:
                    return GloryBurdenReceiver;
                case BurdenReceiverType.M21Receiver:
                    return M21BurdenReceiver;
                case BurdenReceiverType.RaviReceiver:
                    return RaviBurdenReceiver;
                case BurdenReceiverType.SlaughterReceiver:
                    return SlaughterBurdenReceiver;
            }
            return null;
        }


        public static BurdenProcess GetBurdenProcessSender(BurdenSenderType senderType)
        {
            switch(senderType)
            {
                case BurdenSenderType.DEFAULT:
                    return DEFAULTBurdenSender;
                case BurdenSenderType.AlkyoneSender:
                    return AlkyoneBurdenSender;
                case BurdenSenderType.GlorySender:
                    return GloryBurdenSender;
                case BurdenSenderType.M21Sender:
                    return M21BurdenSender;
                case BurdenSenderType.RaviSender:
                    return RaviBurdenSender;
                case BurdenSenderType.SlaughterSender:
                    return SlaughterBurdenSender;
            }
            return null;
        }

        //BURDEN RECEIVER BEHAVIORS
        //BURDEN RECEIVER BEHAVIORS
        //BURDEN RECEIVER BEHAVIORS
        public static BurdenClone DEFAULTBurdenReceiver(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            //the default sender does nothing. The burden is transmitted unmodified.
            Debug.Log("No modifier applied to burden received");
            return burden;
        }

        public static BurdenClone AlkyoneBurdenReceiver(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Burden modified by Alkyone's receiver");
            return burden;
        }

        public static BurdenClone GloryBurdenReceiver(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Burden modified by Glory's receiver");
            return burden;
        }

        public static BurdenClone M21BurdenReceiver(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Burden modified by M21's receiver");
            return burden;
        }

        public static BurdenClone RaviBurdenReceiver(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Burden modified by Ravi's receiver");
            burden.parentBurden.AdjustFear(burden,-1);
            return burden;
        }

        public static BurdenClone SlaughterBurdenReceiver(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Burden modified by Slaughters's receiver");
            return burden;
        }


        //BURDEN SENDER BEHAVIORS
        //BURDEN SENDER BEHAVIORS
        //BURDEN SENDER BEHAVIORS
        public static BurdenClone DEFAULTBurdenSender(BurdenClone burden,BurdenInventory sender, BurdenInventory receiver)
        {
            //the default sender does nothing. The burden is transmitted unmodified.
            Debug.Log("No modifier applied to Burden Sent");
            return burden;
        }

        public static BurdenClone AlkyoneBurdenSender(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Alkyone modifier applied to Burden Sent");
            return burden;
        }

        public static BurdenClone GloryBurdenSender(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Glory modifier applied to Burden Sent");
            return burden;
        }

        public static BurdenClone M21BurdenSender(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("M21 modifier applied to Burden Sent");
            return burden;
        }

        public static BurdenClone RaviBurdenSender(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Ravi modifier applied to Burden Sent");
            return burden;
        }

        public static BurdenClone SlaughterBurdenSender(BurdenClone burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Slaughter modifier applied to Burden Sent");
            return burden;
        }

    }
}
