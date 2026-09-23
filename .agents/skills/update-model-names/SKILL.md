---
name: update-model-names
description: Update model-name constants in AgentFrameworkToolkit provider packages when asked to add current AI models. Use for model catalog refreshes, including OpenRouter equivalents and embedding models; not for provider creation or SDK upgrades.
---

# Update model names

Update the existing provider model constants with verified IDs. Keep the change small and preserve unrelated work.

1. Inspect `AGENTS.md`, the affected `*ChatModels.cs` or `*EmbeddingModels.cs` files, and any code that uses those constants. Check the working tree before editing.
2. Verify each proposed ID against the provider's current official catalog. For OpenRouter constants, verify the exact OpenRouter ID independently; its slugs can differ from the source provider's IDs. Add stable, generally available models by default. Include preview or experimental models only when the user requests them. Do not invent IDs or infer one from a display name. If a provider has no new stable model in scope, leave its constants alone.
3. Follow each class's naming, XML documentation, and `[PublicAPI]` patterns. Preserve existing public constant names and values for compatibility, including older models. Correct inaccurate descriptions where relevant. Update derived lists such as `OpenAIChatModels.ReasoningModels` when new constants affect behavior.
4. Add a short, single changelog bullet describing the model-name update. Keep routine catalog additions out of the root and provider READMEs. Edit a README only when usage instructions change or the user explicitly requests it.
5. Build the affected provider projects and check the diff. Verify that existing constant values remain unchanged and that new OpenRouter IDs exist in its catalog. Do not run `development/` tests unless the user explicitly requests them, as `AGENTS.md` requires.

Report the models added, verification result, and any catalog uncertainty. Link to the catalogs used when citing current model availability.
