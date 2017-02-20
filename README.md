# Poker

`app.config` configuration poker.

Pokes :point_left: values into :point_right: an app.config :+1:

---

:question:

> But why?

Well..... it's probably not useful for that many things, but one thing it can do is to help with e.g. creating a "wizard-like" experience when
configuring a CLI app. :cake: :candy: :doughnut:

Say, for instance, that one creates a nice CLI app called "my-cli", using [GoCommando](https://github.com/rebus-org/GoCommando), which is then merged into one
single binary using [Costura](https://github.com/Fody/Costura), which is then distributed as a single `my-cli.exe` file. Smooth! :cocktail:

The configuration file `my-cli.exe.config` will then hold the configuration settings for the app.

Now you want to provide a special command in your app, so that you can

    C:\> my-app configure

and then prompt the user for a couple of settings, e.g. like a URL and an API key:

    C:\> my-app configure

	Type API URL> http://my-app.azurewebsites.net
	Type API key> fh3u7hf328hf7328fh92hf32f7829

The code that does this could look like this:

    var url = Prompt("Type API URL");
	var key = Prompt("Type API key");

and then we top it off by saving the entered values into `my-cli.exe.config` like this:

    var xml = Poker.AppConfig.LoadXml();
	var poker = new Poker.ConfigurationPoker(xml);

	poker.SetAppSetting("api-url", url);
	poker.SetAppSetting("api-key", key);

	var newXml = poker.RenderXml();

	Poker.AppConfig.SaveXml(newXml);

---

That's it. :clap:




