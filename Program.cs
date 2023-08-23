namespace Mediator
{
	public class Program
	{
		static void Main()
		{
			Mediator mediator = new Mediator();

			Colleague colleague1 = new Colleague(mediator, "colleague1");
			Colleague colleague2 = new Colleague(mediator, "colleague2");
			Colleague colleague3 = new Colleague(mediator, "colleague3");
			Colleague colleague4 = new Colleague(mediator, "colleague4");

			mediator.AddParty(colleague1);
			mediator.AddParty(colleague2);
			mediator.AddParty(colleague3);
			mediator.AddParty(colleague4);

			colleague1.Send("Hello from colleague 1!");
			colleague2.Send("Grretings from Colleague 2!");
			colleague3.Send("Halo from 3!");

			colleague4.Send("Hi 3 and 2", new[]
			{
				colleague2,
				colleague3
			});
		}
	}

	interface IMediator
	{
		void SendMessage(string message, MediatorParty party);
		void SendMessage(string message, MediatorParty party, MediatorParty[]? parties);
		void AddParty(MediatorParty party);
	}

	abstract class MediatorParty
	{
		protected internal readonly IMediator _mediator;

		public MediatorParty(IMediator mediator)
		{
			_mediator = mediator;
		}

		public abstract void Send(string message, MediatorParty[]? parties);
		public abstract void Recieve(string message);
	}

	internal class Colleague : MediatorParty
	{
		public string Name { get; private set; }

		public Colleague(IMediator mediator, string name) : base(mediator)
		{
			Name = name;
		}

        public override void Recieve(string message)
		{
			Console.WriteLine($"{Name} Recieved Message: {message}");
		}

		public override void Send(string message, MediatorParty[]? parties = null)
		{
			Console.WriteLine($"{Name} Sending Message: {message}");
			if (parties == null)
			{
				_mediator.SendMessage(message, this);
			}
			else
			{
				_mediator.SendMessage(message, this, parties);
			}
		}
	}

	internal class Mediator : IMediator
	{
		private readonly List<MediatorParty> _parties;

		public Mediator()
		{
			_parties = new List<MediatorParty>();
		}

		public void AddParty(MediatorParty party)
		{
			_parties.Add(party);
		}

		public void SendMessage(string message, MediatorParty party, MediatorParty[]? parties)
		{
			foreach (var item in _parties)
			{
				if (item != party)
				{
					// If a parties list is provided only send to them.
					if (parties != null && parties.Contains(item) == false)
					{
						continue;
					}

					item.Recieve(message);
				}
			}
		}

		public void SendMessage(string message, MediatorParty party)
		{
			foreach (var item in _parties)
			{
				if (item != party)
				{
					item.Recieve(message);
				}
			}
		}
	}
}
