using System;

namespace conole001
{

    class Interpreter
    {

        public static IO<A> Interpret<A>(IO<A> m)
        {
            switch (m)
            {
                case Return<A> r:
                    return r;

                case IO<CheckCompaniesHouseCmd, ICheckCompaniesHouseResult, A> ch:
                    return Interpret(ch.Next(new CheckCompaniesHousePassed(ch.Input.Name)));

                case IO<CreateCustomerCmd, ICreateCustomerResult, A> cc:
                    return Interpret(cc.Next(new CreateCustomerSuceeded(cc.Input.Name)));

                default:
                    throw new Exception("Unknown Adapter");
            }
        }

    }

}