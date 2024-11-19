import psycopg2
from rasa_sdk import Action, Tracker
from rasa_sdk.executor import CollectingDispatcher
from typing import Any, Text, Dict, List

class ActionFetchSchedule(Action):

    def name(self) -> Text:
        return "action_fetch_schedule"

    def run(self, dispatcher: CollectingDispatcher,
            tracker: Tracker,
            domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:
        
        # Extract the sport from the user's message
        sport = next(tracker.get_latest_entity_values("sport"), None)
        
        if not sport:
            dispatcher.utter_message(text="Please specify the sport you are interested in.")
            return []

        # Connect to PostgreSQL database
        connection = psycopg2.connect(
            user="postgres",
            password="user123",
            host="127.0.0.1",
            port="5432",
            database="FYPTourneyPro"
        )
        cursor = connection.cursor()

        # Query the tournament schedule for the specified sport
        query = "SELECT sport, date, location FROM tournament_schedule WHERE sport = %s ORDER BY date LIMIT 1"
        cursor.execute(query, (sport,))
        record = cursor.fetchone()

        # Format the response
        if record:
            response = f"The next {record[0]} competition is on {record[1]} at {record[2]}."
        else:
            response = f"Sorry, I couldn't find any upcoming {sport} competitions."

        # Close the database connection
        cursor.close()
        connection.close()

        # Send the response back to the user
        dispatcher.utter_message(text=response)
        return []