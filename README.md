# CommuterAssistant

💬 Little chat based assistant to help me catch the next bus to my next
destination

> **Disclaimer**
>
> This project is developed solely as a toy project and is not aimed for
> anything more than that. I do not plan to support multiple countries, a
> user base or a publicly reachable version of this bot.

## Getting started

### Telegram

To get your [Telegram](https://telegram.org) API key, send a message to
[BotFather](https://t.me/BotFather). Once its done you should have a token that
looks something like `123456:ABC-DEF1234ghIkl-zyx57W2v1u123ew11`, paste it in
along with your username like in the following example:

[`Assistant/Assistant/appsettings.json`](./Assistant/Assistant/appsettings.json):

```json
{
  "BotConfiguration": {
    "ApiKey": "123456:ABC-DEF1234ghIkl-zyx57W2v1u123ew11",
    "AllowedUsers": ["your-telegram-username"]
  }
}
```

> Your API key and all of your data will remain private, no data is ever shared
> outside of your application with any database or any third party system except
> the [Telegram Bot API client](https://github.com/TelegramBots/telegram.bot).

Once you've set up your credentials, run the run the project `Assistant` and
start messaging your your bot. You should see your messages logged in your
console.

### Discord

> Not yet developed

## Planned features

> Feel free to help with any of those !

- [x] Telegram integration
- [x] Some kind of persistence
- [ ] Send back the next departure
- [ ] Unit testing
- [ ] CI with GitHub Actions
- [ ] Discord integration
- [ ] Support different sources simultaneously (ex: update your home location
  from Discord and ask for the next departure from Telegram)
