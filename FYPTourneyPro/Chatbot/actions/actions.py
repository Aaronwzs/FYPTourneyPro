import psycopg2
from rasa_sdk import Action, Tracker
from rasa_sdk.executor import CollectingDispatcher
from typing import Any, Text, Dict, List
from rasa_sdk.events import SlotSet
import logging

class ActionFetchSchedule(Action):
    def name(self) -> Text:
        return "action_fetch_schedule"

    def run(self, dispatcher, tracker, domain):
        # Get dynamic inputs from slots
        player_name = tracker.get_slot("player")
        tournament_name = tracker.get_slot("tournament_name")

        if not (player_name or tournament_name):
            dispatcher.utter_message("Please provide a player name or tournament name.")
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

        # Build query dynamically based on inputs
        if player_name:
            query = """
            SELECT m."startTime", m."endTime", t."Name", u."NormalizedUserName"
            FROM "Match" m
            JOIN "MatchParticipant" mp ON m."Id" = mp."MatchId"
            JOIN "AbpUsers" u ON mp."UserId" = u."Id"
            JOIN "Category" c ON m."CategoryId" = c."Id"
            JOIN "Tournament" t ON c."TournamentId" = t."Id"
            WHERE u."NormalizedUserName" = %s
            """
            cursor.execute(query, (player_name,))
        elif tournament_name:
            query = """
            SELECT m."startTime", m."endTime", t."Name", u."NormalizedUserName"
            FROM "Match" m
            JOIN "MatchParticipant" mp ON m."Id" = mp."MatchId"
            JOIN "AbpUsers" u ON mp."UserId" = u."Id"
            JOIN "Category" c ON m."CategoryId" = c."Id"
            JOIN "Tournament" t ON c."TournamentId" = t."Id"
            WHERE t."Name" = %s
            """
            cursor.execute(query, (tournament_name,))

        results = cursor.fetchall()
        connection.close()

        # Format and return results
        if results:
            players = set()
            response = []
            for row in results:
                start_time, end_time, tournament_name, player_name = row
                players.add(player_name)
                match_info = f"- Tournament: {tournament_name}\n  Start Time: {start_time}\n  End Time: {end_time}"
                response.append(match_info)

            response.append("\nPlayers: " + ", ".join(players))
            dispatcher.utter_message("\n".join(response))
        else:
            dispatcher.utter_message("No matches found for the provided details.")

        return []


class ActionFetchNextMatch(Action):
    def name(self) -> str:
        return "action_fetch_next_match"

    def run(self, dispatcher, tracker, domain):
        

        # Fetch the user's next match (placeholder logic)
        dispatcher.utter_message(text=f"Fetching next match for user ID: {user_id}.")
        return []


class ActionExtractCustomData(Action):
    def name(self) -> str:
        return "action_extract_custom_data"

    def run(self, dispatcher, tracker, domain):
        # Extract metadata from the latest message
        metadata = tracker.latest_message.get("metadata", {})
        user_id = metadata.get("userId")
        is_logged_in = metadata.get("isLoggedIn", False)

        # Debug: Log extracted values (optional)
        dispatcher.utter_message(
            text=f"Extracted metadata -> UserId: {user_id}, IsLoggedIn: {is_logged_in}"
        )

        # Set slots dynamically
        return [
            SlotSet("userId", user_id),
            SlotSet("isLoggedIn", is_logged_in),
        ]