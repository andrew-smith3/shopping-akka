using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;

namespace ShoppingCart.WebApi
{
    public class AspNetCoreDependencyResolver : IDependencyResolver
    {
        private IServiceProvider _serviceProvider;
        private ActorSystem _actorSystem;

        public AspNetCoreDependencyResolver(IServiceProvider serviceProvider, ActorSystem actorSystem)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _actorSystem = actorSystem ?? throw new ArgumentNullException(nameof(actorSystem));
            _actorSystem.AddDependencyResolver(this);
        }

        public Type GetType(string actorName)
        {
            return typeof(string);
        }

        public Func<ActorBase> CreateActorFactory(Type actorType)
        {
            return null;
        }

        public Props Create<TActor>() where TActor : ActorBase
        {
            return Create(typeof(TActor));
        }

        public Props Create(Type actorType)
        {
            return _actorSystem.GetExtension<DIExt>().Props(actorType);
        }

        public void Release(ActorBase actor)
        {
            //not sure what to release
        }
    }
}
