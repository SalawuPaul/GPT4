// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.22.0

using GPT4.Models;
using GPT4.Services.OpenAI;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GPT4.Bots
{
    public class GPT4Bot : ActivityHandler
    {

        private readonly IOpenAIService _openAIService;
        private UserState _userstate;
        private ConversationState _conversationState;

        public GPT4Bot(IOpenAIService openAIService, UserState userState, ConversationState conversationState)
        {
            _openAIService = openAIService;
            _userstate = userState;
            _conversationState = conversationState;

        }
        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);
            await _userstate.SaveChangesAsync(turnContext, false, cancellationToken);
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var conversationAccessor = _conversationState.CreateProperty<List<ChatMessages>>("lastMessages");
            var lastMessages = await conversationAccessor.GetAsync(turnContext, () => new List<ChatMessages>(), cancellationToken);

            var response = await _openAIService.ChatCompletionAsync(turnContext.Activity.Text, lastMessages);

            await turnContext.SendActivityAsync(MessageFactory.Text(response.BotMessage, response.BotMessage), cancellationToken);
            if (lastMessages.Count >= 5)
            {
                lastMessages.RemoveAt(lastMessages.Count - 1);
            }
            lastMessages.Add(response);
            await conversationAccessor.SetAsync(turnContext, lastMessages);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello! welcome to GPT4 Bot!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
