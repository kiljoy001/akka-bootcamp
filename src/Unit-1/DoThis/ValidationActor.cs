using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTail
{
    class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if(string.IsNullOrEmpty(msg))
            {
                //signal that the user needs to supply as an input
                _consoleWriterActor.Tell(
                    new Messages.NullInputError("No input recieved"));
            }

            else
            {
                var valid = IsValid(msg);
                if(valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Message Valid"));
                }

                else
                {
                    _consoleWriterActor.Tell(new Messages.ValidationError("Input has odd number of characters"));
                }
            }   
        }

        private static bool IsValid(string msg)
        {
            var valid = msg.Length % 2 == 0;
            return valid;
        }
    }
}
