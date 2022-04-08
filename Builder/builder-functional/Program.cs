using System;
using System.Linq;
using System.Collections.Generic;

namespace builder_functional
{
    public class Person{
        public string Name, Position;
    }
    public abstract class FunctionalBuilder<TSubject, TSelf>
        where TSubject : new()
        where TSelf: FunctionalBuilder<TSubject, TSelf>
        {
            private readonly List<Func<TSubject, TSubject>> actions
                =new List<Func<TSubject, TSubject>>();
            
           
            
            public TSelf Do(Action<TSubject> action)
                => AddAction(action);
            
            public TSubject Build()
                => actions.Aggregate(new TSubject(), (p,f) => f(p));
            
            private TSelf AddAction(Action<TSubject> action)
            {
                actions.Add(p => {action(p);
                    return p;    
                });
                return (TSelf) this;
            }

        }
        public sealed class PersonBuilder
            : FunctionalBuilder<Person, PersonBuilder>
            {
                public PersonBuilder Called(string name)
                    => Do(p => p.Name = name);
            }


    class Program
    {
        static void Main(string[] args)
        {
            var person = new PersonBuilder()
                .Called("name")
                .Build();
           
        }
    }
}
