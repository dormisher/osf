using System;
using System.Collections.Generic;
using System.Web.Mvc;
using osf.web.Models;
using osf.web.Services;

namespace osf.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventService _eventService = new EventService();

        public ActionResult Index()
        {
            var model = new HomeModel {LatestEvents = _eventService.LoadEvents(4), Quote = GetQuote()};

            return View(model);
        }

		private QuoteModel GetQuote()
		{
			var quotes = new List<QuoteModel>();

			quotes.Add(new QuoteModel
				{
					Person = "Chris Holland",
					Info = "Footballer with Newcastle United",
					Quote = "Having seen the objectives of OSF and its charitable works, I felt so strongly about their work that I asked if I could get involved on the coaching side. Having grown up around people from underprivileged backgrounds I have seen first hand how sport can make a huge difference to peoples' lives. I hope to be able to provide youngsters with a chance to enjoy similar experiences to myself."
				});
			quotes.Add(new QuoteModel
				{
					Person = "Graeme \"Dezzi\" Higginson",
					Info = "British Masters Boxing Champion",
					Quote = "I have witnessed life changing experiences through all sports, especially boxing. Helping disadvantaged young people is critically important if they are to fullfil their potential and contribute fully to society. Providing these people with sports coaching is a solid base to build from. The work OSF carries out has, in my opinion, provided much needed support to these youngsters."
				});
			quotes.Add(new QuoteModel
				{
					Person = "Graeme Hawley",
					Info = "Coronation Street Actor",
					Quote = "It is getting harder and harder for young people, particularly those who are disadvantaged, to grow up and escape the more negative influences infecting our streets. Organisations such as OSF who are focused on the provision of positive role models can only be a good thing in my opinion and I fully support what Opportunity Sports Foundation are trying to achieve."
				});
			quotes.Add(new QuoteModel
				{
					Person = "Abigail Wilson, age 15",
					Info = "Participant in OSF Activity",
					Quote = "It was the first time I had ever done Circuit Training and it was great. It was good fun but hard work, I hope I can continue doing sport now."
				});
			quotes.Add(new QuoteModel
				{
					Person = "Waqar Patel, age 17",
					Info = "Participant in OSF Activity",
					Quote = "I thought I was too old to enjoy sport but it was brilliant. The coaching that Opportunity Sports Foundation gave us was really good, can't wait untill the next session."
				});
			quotes.Add(new QuoteModel
				{
					Person = "Shaun Beeley",
					Info = "Oldham Athletic, Southport & Fleetwood Footballer",
					Quote = "Sport changed my life. Only a few years ago I was comitting crime and known to the police. Coaches at OSF helped me concentrate on football and now I am a professional. I owe them a lot and cannot thank them enough."
				});
			quotes.Add(new QuoteModel
				{
					Person = "Pelle Mathausa",
					Info = "Lancashire Football Association",
					Quote = "We at the Lancashire Football Association support all development of young people through sport. I personally believe that free sports coaching will increase performance levels of our young sports people and will definitely increase the numbers of participants."
				});

			var random = new Random((int)DateTime.Now.Ticks);

			return quotes[random.Next(0, quotes.Count)];
		}
	}
}