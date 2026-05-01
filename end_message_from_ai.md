### Iteration 1

I have successfully implemented the prototype for the Subtitle QC system based on the specifications in iteration_1.md and the constraints in agents_iterative.md.

Architectural Implementation Details
Internal Data Model:
Created a unified Cue class (SubtitleQc.Core/Models/Cue.cs) that acts as the agnostic internal representation. It holds the Id, Start and End TimeSpans, and a list of Lines.
Parsers:
Implemented ISubtitleParser interface (SubtitleQc.Core/Parsers/ISubtitleParser.cs).
Implemented SrtParser and WebVttParser that parse raw string content into the internal Cue model. These are completely decoupled from the Rule Engine.
Rule Engine & Rules:
Implemented IQcRule interface to ensure the Open/Closed Principle (OCP).
Created individual rule classes for each acceptance criteria: MaxLinesRule, MaxCplRule, MaxCpsRule, MinDurationRule, OverlapCheckRule, and EmptyCueCheckRule. Each rule is responsible for a single validation concern (Single Responsibility Principle).
Implemented RuleEngine that iterates over the provided rules and aggregates the results into a QcReport containing QcResults for each cue.
Technical Constraints:
Created a SubtitleQc.Core class library targeting .NET 10.
Adhered to the strict rule of one class per file.
Refactored complex logic to ensure no method exceeds 20 lines of code.
Added a Directory.Packages.props file to centrally manage NuGet package versions, allowing the test suite to restore and run successfully without modifying the test project itself.
Telemetry:
Logged all dotnet test execution attempts to test_metrics.jsonl in the root directory as required by the telemetry protocol.
All 13 acceptance criteria tests in the SubtitleQc.Tests project are now passing successfully.

### Iteration 2

I have successfully expanded the prototype to support TTML formats according to the specifications in iteration_2.md.

Architectural Implementation Details
Parsers Expansion:

Implemented a new TtmlParser class (SubtitleQc.Core/Parsers/TtmlParser.cs) that implements the ISubtitleParser interface.
The parser utilizes System.Xml.Linq to parse the XML-based TTML structure.
It is designed to be robust: it falls back to parsing any namespace if the standard TTML namespace isn't used, handles inline elements like <br/> for line breaks, and recursively extracts text from other inline elements (like <span>).
It correctly maps TTML begin and end attributes to TimeSpan objects and xml:id or id attributes to the internal Cue ID.
Preservation of Logic:

The core RuleEngine and all existing rules (MaxLinesRule, MaxCplRule, MaxCpsRule, MinDurationRule, OverlapCheckRule, EmptyCueCheckRule) remain completely untouched.
Because the TtmlParser converts the TTML data into the unified, agnostic Cue internal data model, the existing rules seamlessly validate TTML files without any modifications.
The Open/Closed Principle (OCP) and Single Responsibility Principle (SRP) are maintained.
Technical Constraints:

The new parser strictly adheres to the one class per file rule.
Complex XML parsing logic was refactored into smaller, focused private helper methods (ParseCue, ExtractTextAndBr, TryParseTime) to ensure no method exceeds 20 lines of code.
All 13 existing acceptance criteria tests continue to pass successfully, confirming that the core logic and existing format support were not broken.
Telemetry:

Logged the dotnet test execution attempt for iteration 2 to test_metrics.jsonl in the root directory.

### Iteration 3

I have successfully expanded the prototype to support external shot change data integration according to the specifications in iteration_3.md.

Architectural Implementation Details
Internal Data Model Upgrade:

Extended the Cue class (SubtitleQc.Core/Models/Cue.cs) to include an optional StartFrame integer property. This allows the unified model to represent frame-based timing when available, without breaking backward compatibility for parsers that only provide TimeSpan data.
External Data Integration Abstraction:

Created the IShotChangeProvider interface (SubtitleQc.Core/Qc/Abstractions/IShotChangeProvider.cs). This adheres to the Dependency Inversion Principle (DIP) by ensuring the rules depend on an abstraction rather than a concrete implementation of external data loading. It provides methods to retrieve both timestamp-based cuts and frame-based cuts.
Rule Engine Expansion:

Implemented CrossShotBoundaryCheckRule (SubtitleQc.Core/Qc/Rules/CrossShotBoundaryCheckRule.cs): This rule takes an IShotChangeProvider and fails any cue whose Start and End times span across an external cut timestamp.
Implemented MinFramesFromShotChangeRule (SubtitleQc.Core/Qc/Rules/MinFramesFromShotChangeRule.cs): This rule takes an IShotChangeProvider and a threshold, and fails any cue whose StartFrame is within the threshold distance of any external cut frame.
Technical Constraints:

The new abstractions and rules strictly adhere to the one class per file rule.
Complex logic was kept minimal and declarative using LINQ, ensuring no method exceeds 20 lines of code.
The existing test suite was respected, and all 19 tests (including the 6 new tests for Iteration 3) are now passing successfully.
Telemetry:

Logged the dotnet test execution attempt for iteration 3 to test_metrics.jsonl in the root directory.