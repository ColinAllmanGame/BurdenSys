using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoStudios.Burdens
{
    public delegate Burden BurdenProcess(Burden burden, BurdenInventory sender, BurdenInventory receiver);


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

        public enum TransactionRejectionReasons
        {
            CharacterUnableToReceive,
            CharacterUnableToSend,
            DuplicatesPrevented,
        }
    

        public static bool TransferBurden(Burden burden,CharacterBurdenManager sender, CharacterBurdenManager receiver)
        {

            //burden.BurdenPreSend(sender); //burden prepares for dispatch while inside first container. 

            //Strongly consider verifying the ability for sender and receiver to "pre-approve" the transfer. As it is very hard to undo.
            //it would be nice to get a reason for the rejection, perhaps return some send/receive data on prevalidate.

            if(!PrevalidateTransaction(burden, sender, receiver))
            {
                return false;
            }


            bool SendSuccess = sender.burdenInventory.DispatchBurden(burden,sender.burdenInventory,receiver.burdenInventory);
            bool ReceiveSuccess = receiver.AddBurden(burden, sender, receiver);

            if(SendSuccess && ReceiveSuccess)
            {
                Debug.Log("Burden transaction complete");
                return true;
            }
            else
            {
                Debug.Log("Burden transaction failure to send from " + sender.burdenInventory.ContainerName + " to " + receiver.burdenInventory.ContainerName +
                    " of type " + burden.category.ToString());
                return false;
            }

        }

        public static bool PrevalidateTransaction(Burden burden, CharacterBurdenManager sender, CharacterBurdenManager receiver)
        {
            //if receiver is null, this is auto-true, as we are destroying the burden
            bool canReceive = true;

            //if sender is null, this is auto-true, as this is a new burden from the world.;
            bool canSend = true;

            if (sender != null)
            {
                if (!sender.canSendBurdens)
                {
                    canSend = false;
                }
            }
            if(receiver!=null)
            {
                if(!receiver.canReceiveBurdens)
                {
                    canReceive = false;
                }
            }


            if (burden.duplicateRule == Burden.CloneDuplicateRule.rejectAllDuplicateRequests && receiver.burdenInventory.d_heldBurdens.ContainsKey(burden.category))
            {
                Debug.Log("burden duplicate disallowed by configuration of parent burden. (category type was already held)");
                //this is not necessarily a failure, as some burdens may accumulate while others cannot. Abort the transaction.
                //be wary, this can infinite loop if paired with an aggressive queue.
                //let the sender know their attempt was rejected, perhaps even add more flavorful data later.
                sender.BurdenSendRejected(TransactionRejectionReasons.DuplicatesPrevented, sender, receiver, burden);
                receiver.BurdenReceiveRejected(TransactionRejectionReasons.DuplicatesPrevented, sender, receiver, burden);
                return false;
            }

            if (!canSend)
            {
                Debug.LogWarning("A burden transaction was rejected by SENDER : " + sender.burdenInventory.ContainerName);
                sender.BurdenSendRejected(TransactionRejectionReasons.CharacterUnableToSend, sender, receiver,burden);
                receiver.BurdenReceiveRejected(TransactionRejectionReasons.CharacterUnableToSend, sender, receiver,burden);
                BurdenTransactionFailedLog(burden, sender, receiver, false);
                return false;
            }
            if (!canReceive)
            {
                Debug.LogWarning("A burden transaction was rejected by RECEIVER : " + receiver.burdenInventory.ContainerName);
                sender.BurdenSendRejected(TransactionRejectionReasons.CharacterUnableToReceive, sender, receiver, burden);
                receiver.BurdenReceiveRejected(TransactionRejectionReasons.CharacterUnableToReceive, sender, receiver, burden);
                BurdenTransactionFailedLog(burden, sender, receiver, false);
                return false;
            }
            return true;
        }

        public static void BurdenTransactionFailedLog(Burden burden, CharacterBurdenManager sender, CharacterBurdenManager receiver, bool queue = false)
        {
            //this is for error debugging, and not behavior.
            Debug.LogWarning("A burden transaction failed between " + sender.burdenInventory.ContainerName + " and " + receiver.burdenInventory.ContainerName);
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
        public static Burden DEFAULTBurdenReceiver(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            //the default sender does nothing. The burden is transmitted unmodified.
            Debug.Log("No modifier applied to burden received");
            return burden;
        }

        public static Burden AlkyoneBurdenReceiver(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Burden modified by Alkyone's receiver");
            return burden;
        }

        public static Burden GloryBurdenReceiver(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Burden modified by Glory's receiver");
            return burden;
        }

        public static Burden M21BurdenReceiver(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Burden modified by M21's receiver");
            return burden;
        }

        public static Burden RaviBurdenReceiver(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Burden modified by Ravi's receiver");
            burden.parentBurden.AdjustFear(-1);
            return burden;
        }

        public static Burden SlaughterBurdenReceiver(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Burden modified by Slaughters's receiver");
            return burden;
        }


        //BURDEN SENDER BEHAVIORS
        //BURDEN SENDER BEHAVIORS
        //BURDEN SENDER BEHAVIORS
        public static Burden DEFAULTBurdenSender(Burden burden,BurdenInventory sender, BurdenInventory receiver)
        {
            //the default sender does nothing. The burden is transmitted unmodified.
            Debug.Log("No modifier applied to Burden Sent");
            return burden;
        }

        public static Burden AlkyoneBurdenSender(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Alkyone modifier applied to Burden Sent");
            return burden;
        }

        public static Burden GloryBurdenSender(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Glory modifier applied to Burden Sent");
            return burden;
        }

        public static Burden M21BurdenSender(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("M21 modifier applied to Burden Sent");
            return burden;
        }

        public static Burden RaviBurdenSender(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Ravi modifier applied to Burden Sent");
            return burden;
        }

        public static Burden SlaughterBurdenSender(Burden burden, BurdenInventory sender, BurdenInventory receiver)
        {
            Debug.Log("Slaughter modifier applied to Burden Sent");
            return burden;
        }

    }
}
