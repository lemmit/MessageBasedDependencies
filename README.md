# "Message Based Dependencies"
Message Based Dependencies project is another try to create a strongly typed, DI free object system.  
It can be seen as second part of an experiment, how C# type system works and how we can implement it non-casual ways of dependency resolving/injection.
  
The idea is to allow classes to specify their dependencies explicit, without using constructor injection.  
So to use:  
```
public YourClass : BaseObject,   
  ISubscriber<MessageOne>, ISubscriber<MessageTwo>,  
  IPublisher<MessageThree> 
  {   
    ...handlers implementation (forced) here...  
  }  
```
instead of:  
```
public class YourClass   
{  
    public YourClass(IFirstDependency dep, ISecondDependency, ....., IMilionAndForthDependency)  
    {  
      ....  
    }  
}
```

Thanks to that we don't have to use IoC - the only dependency is some kind of common bus for sending/receiving messages.  
That way the whole system is more scalable (not dependent on the implementation of the bus also!) and resilent.  
E.g. in case of non-existent handler system should work - if service is not available in IoC we'd meet NullPointerException.

## Tests
Example of master-slave cooperation in a given model is provided. Using command line parameters some execution params can be changed - fell free to test it! I've done some performance tests - using asynchronous bus (and therefor parallel execution) and ~8 slave workers the execution times were up to ~2x lower than with synchronous execution.

And just like in case of previous experiment ([my try to implement real OOP system in C#](https://github.com/lemmit/RealOOP)) 
the system I've created looks more and more like an actor system. 
Final way of implementing classes looks almost exacly the same like TypedActor in Akka.Net!  
I'll end with just a simple tought that Actors are the new OOP :)
