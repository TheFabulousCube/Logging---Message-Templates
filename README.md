# Comparative Analysis of Structured Logging vs. Interpolated String Logging in AWS CloudWatch

## Overview

This repository contains a test program to compare structured logging and interpolated string logging using Serilog and AWS CloudWatch.

## Structured Logging vs. Interpolated String Logging

### Structured Logging

#### Example

```C#
  foreach (string color in colors)
  {
      foreach (string shape in shapes)
      {
          colorsAndShapesLogger.LogInformation("A {Color} {Shape}", color, shape);
      }
  }
```

#### Advantages

1. **Enhanced Querying**: Allows querying logs based on specific properties.
2. **Better Log Analysis**: Tools can parse structured logs more effectively.
3. **Consistency**: Maintains a consistent format.
4. **Contextual Information**: Includes additional context information.
5. **Performance**: More efficient for searching and querying in CloudWatch.

#### Disadvantages

1. **Complexity**: More complex to implement.
2. **Performance Overhead**: Slight performance overhead due to serialization.

#### CloudWatch Query Examples

- **Filter by SourceContext and Color**

```
  fields @timestamp, @message
  | filter Properties.SourceContext = "Colors and Shapes"
  | filter Properties.Color = "White"
  | sort @timestamp desc
```

- **Count Entries by Shape**

```
  fields @timestamp, Properties.Shape
  | filter Properties.SourceContext = "Colors and Shapes"
  | stats count() by Properties.Shape
  | sort @timestamp desc
```

- **Find Errors in Specific Context**

```
  fields @timestamp, @message
  | filter Properties.SourceContext = "Colors and Shapes"
  | filter @message like "error"
  | sort @timestamp desc
```




### Interpolated String Logging

#### Example
```C#
  foreach (string color in colors)
  {
      foreach (string shape in shapes)
      {
          colorsAndShapesLogger.LogInformation($"A {color} {shape}");
      }
  }
```
#### Advantages

1. **Simplicity**: Straightforward and easy to implement.
2. **Readability**: Human-readable log messages.
3. **Performance**: Can be faster in some cases.

#### Disadvantages

1. **Limited Querying**: Harder to query based on specific properties.
2. **Inconsistent Format**: Format can vary.
3. **Lack of Context**: May lack additional context information.

#### CloudWatch Query Examples  
- **Filter by Message Content**

```
fields @timestamp, @message
| filter @message like "Colors and Shapes" and @message like "White"
| sort @timestamp desc
```

- **Count Entries by Keyword**

```
fields @timestamp, @message
| filter @message like "Shape"
| stats count() by @message
| sort @timestamp desc
```


## Conclusion

Both structured logging and interpolated string logging have their own advantages and disadvantages. However, structured logging provides significant benefits in terms of querying and analyzing logs, making it a preferred choice for applications that require detailed log analysis.
