﻿using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArrowverseBot
{
	class Config
	{
		private const string configFolder = "Resources";
		private const string configFile = "config.json";
		private const string triviaQuestionsFile = "trivia_questions.json";

		public static BotConfig bot;
		public static TriviaQuestions triviaQuestions;
		public static Minigames.Trivia Trivia = new Minigames.Trivia();

		static Config()
		{
			if (!Directory.Exists(configFolder))
				Directory.CreateDirectory(configFolder);

			if (!File.Exists($"{configFolder}/{configFile}"))
			{
				string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
				File.WriteAllText($"{configFolder}/{configFile}", json);
			}
			else
			{
				string json = File.ReadAllText($"{configFolder}/{configFile}");
				bot = JsonConvert.DeserializeObject<BotConfig>(json);
			}

			if (!File.Exists($"{configFolder}/{triviaQuestionsFile}"))
			{
				string json = JsonConvert.SerializeObject(triviaQuestions, Formatting.Indented);
				File.WriteAllText($"{configFolder}/{triviaQuestionsFile}", json);
			}
			else
			{
				string json = File.ReadAllText($"{configFolder}/{triviaQuestionsFile}");
				triviaQuestions = JsonConvert.DeserializeObject<TriviaQuestions>(json);
			}
		}
	}

	public struct BotConfig
	{
		public string DisordBotToken;
	}

	public struct TriviaQuestion
	{
		public string Question;
		public string Answer;
		public List<string> IncorrectAnswers;
	}
	public struct TriviaQuestions
	{
		public List<TriviaQuestion> Questions;
	}
}