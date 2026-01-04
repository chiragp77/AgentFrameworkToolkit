using System.Text;
using AgentFrameworkToolkit.OpenAI;
using AgentFrameworkToolkit.OpenRouter;
using AgentFrameworkToolkit.Tools;
using AgentSkillsDotNet;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Secrets;

namespace Sandbox.Providers;

public static class OpenRouter
{
    static string GetWeather(string city)
    {
        return "{ \"condition\": \"sunny\", \"degrees\":19 }";
    }

    public static async Task RunAsync()
    {
        Secrets.Secrets secrets = SecretsManager.GetSecrets();
        OpenRouterAgentFactory factory = new(new OpenRouterConnection
        {
            ApiKey = secrets.OpenRouterApiKey
        });
        AgentSkillsFactory agentSkillsFactory = new AgentSkillsFactory();
        AgentSkills agentSkills = agentSkillsFactory.GetAgentSkills("TestData\\AgentSkills");

        foreach (AgentSkill skill in agentSkills.Skills)
        {
            //Validate the skill against the official spec
            AgentSkillValidationResult validationResult = skill.GetValidationResult();

            //Get Skill definition
            string definition = skill.GenerateDefinition(new AgentSkillAsToolOptions
            {
                //add your optional options for how the definition is generated
            });

            //Get as AI Tool
            AITool tool = skill.AsAITool(new AgentSkillAsToolOptions
            {
                //add your optional options for how the definition is generated
            });

            /* Default Definition Example
            <skill name="speak-like-a-pirate" description="Let the LLM take the persona of a pirate">
                <instructions>
                    # Speak Like a pirate## ObjectiveSpeak Like a pirate called 'Seadog John' ...
                    He has a parrot called 'Squawkbeard'

                    ## Context
                    This is a persona aimed at kids that like pirates

                    ## Rules
                    - Use as many emojis as possible
                    - As this need to be kid-friendly, do not mention alcohol and smoking
                </instructions>
            </skill>
            */
        }

/* Instructions will return in the Anthropic preferred format

<available_skills>
     <skill>
       <name>pdf-processing</name>
       <description>Extracts text and tables from PDF files, fills forms, merges documents.</description>
       <location>/path/to/skills/pdf-processing/SKILL.md</location>
     </skill>
     <skill>
       <name>data-analysis</name>
       <description>Analyzes datasets, generates charts, and creates summary reports.</description>
       <location>/path/to/skills/data-analysis/SKILL.md</location>
     </skill>
   </available_skills>
 */


//Expose Skills as 3 tools ('get-available-skills', 'get-skill-by-name' and 'read-skill-file-content')
        IList<AITool> tools1 = agentSkills.GetAsTools(AgentSkillsAsToolsStrategy.AvailableSkillsAndLookupTools);

//Expose each skill as its own tool (+ 'read-skill-file-content' tool)
        IList<AITool> tools2 = agentSkills.GetAsTools(AgentSkillsAsToolsStrategy.EachSkillAsATool);

//Control every option in the tools creation
        IList<AITool> tools3 = agentSkills.GetAsTools(AgentSkillsAsToolsStrategy.AvailableSkillsAndLookupTools, new AgentSkillsAsToolsOptions
        {
            IncludeToolForFileContentRead = false, //Exclude a 'read-skill-file-content' tool

            //Override default tool names/descriptions
            GetAvailableSkillToolName = "get-skills",
            GetAvailableSkillToolDescription = "Get all the skills",
            GetSpecificSkillToolName = "get-skill",
            GetSpecificSkillToolDescription = "Get a specific tool",
            ReadSkillFileContentToolName = "read-file",
            ReadSkillFileContentToolDescription = "Read a skill file",

            //Control how each Skill report it's content back (XML Structure)
            AgentSkillAsToolOptions = new AgentSkillAsToolOptions
            {
                IncludeDescription = true,
                IncludeAllowedTools = true,
                IncludeMetadata = true,
                IncludeLicenseInformation = true,
                IncludeCompatibilityInformation = true,
                IncludeScriptFilesIfAny = true,
                IncludeReferenceFilesIfAny = true,
                IncludeAssetFilesIfAny = true,
                IncludeOtherFilesIfAny = true,
            }
        });


        string instructions = agentSkills.GetInstructions(); //Get instructions of available skills

        IList<string> log = agentSkills.ExcludedSkillsLog; //Log of skills that where exclude (due to being invalid or filtered away by advanced filtering)

        string skillsInstructions = agentSkills.GetInstructions();

        IList<AITool> tools = agentSkills.GetAsTools();
        tools.Add(AIFunctionFactory.Create(PythonRunner.RunPhytonScript, name: "execute_python"));

        OpenRouterAgent agent = factory.CreateAgent(
            new AgentOptions
            {
                Model = OpenRouterChatModels.OpenAI.Gpt41Mini,
                Instructions = $"""
                                You are an nice a AI with various skills
                                ## Skills available:
                                {skillsInstructions}

                                Only call '{AgentSkillsAsToolsOptions.DefaultReadSkillFileContentToolName}' tool once you have used '{AgentSkillsAsToolsOptions.DefaultGetSpecificSkillToolName}' tool
                                """,
                Tools = tools,
                RawToolCallDetails = details =>
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(details.ToString());
                    Console.ResetColor();
                }
            }
        );

        AgentThread thread = agent.GetNewThread();

        Console.OutputEncoding = Encoding.UTF8;
        while (true)
        {
            Console.Write("> ");
            string message = Console.ReadLine() ?? "";
            AgentRunResponse response = await agent.RunAsync(message, thread);
            Console.WriteLine(response);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Input Tokens: {response.Usage!.InputTokenCount} - Output Tokens: {response.Usage.OutputTokenCount} ");
            Console.ResetColor();
        }
    }
}
