
main.py
import asyncio
from autogen_agentchat.agents import AssistantAgent
from autogen_agentchat.ui import Console
from autogen_ext.models.openai import OpenAIChatCompletionClient
from autogen_ext.tools.langchain import LangChainToolAdapter
from langchain_experimental.tools.python.tool import PythonAstREPLTool

async def main():
    # Initialize the DataFrame
    df = pd.read_csv("your_file.csv")

    # Define the preprocessing functions
    def preprocess_data(df):
        df = standardize_numerical(df)
        df = encode_categorical(df)
        df = map_boolean(df)
        df = extract_datetime_features(df)
        df = vectorize_text(df)
        return df

    # Wrap the preprocessing function with LangChain
    tool = LangChainToolAdapter(PythonAstREPLTool(locals={"df": df, "preprocess_data": preprocess_data}))

    # Initialize the assistant agent
    model_client = OpenAIChatCompletionClient(model="gpt-4o")
    assistant = AssistantAgent("assistant", tools=[tool], model_client=model_client, system_message="Use the `df` variable to access the dataset.")

    # Run the assistant
    await Console(assistant.run_stream(task="Standardize the dataset"))

asyncio.run(main())

textvector.py
  from sklearn.feature_extraction.text import TfidfVectorizer

  def vectorize_text(df):
      text_cols = df.select_dtypes(include=['object']).columns
      vectorizer = TfidfVectorizer(max_features=100)
      for col in text_cols:
          tfidf_matrix = vectorizer.fit_transform(df[col].fillna(''))
          tfidf_df = pd.DataFrame(tfidf_matrix.toarray(), columns=vectorizer.get_feature_names_out())
          df = df.drop(columns=[col]).join(tfidf_df)
      return df

boolean.py
  def map_boolean(df):
      boolean_cols = df.select_dtypes(include=['bool']).columns
      df[boolean_cols] = df[boolean_cols].astype(int)
      return df

      from sklearn.preprocessing import OneHotEncoder

categorical.py

  def encode_categorical(df):
      encoder = OneHotEncoder(drop='first', sparse=False)
      categorical_cols = df.select_dtypes(include=['object']).columns
      encoded = encoder.fit_transform(df[categorical_cols])
      encoded_df = pd.DataFrame(encoded, columns=encoder.get_feature_names_out(categorical_cols))
      df = df.drop(columns=categorical_cols).join(encoded_df)
      return df

stadarscaller.py
  from sklearn.preprocessing import StandardScaler

  def standardize_numerical(df):
      scaler = StandardScaler()
      numerical_cols = df.select_dtypes(include=['float64', 'int64']).columns
      df[numerical_cols] = scaler.fit_transform(df[numerical_cols])
      return df
