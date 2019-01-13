#include <iostream>
#include "Socket.h"

using namespace std;

int main() 
{
	int nChoice;
	int port = 24242;
	string ipAdress;
	char reciveMessage[MAXSTRLEN];
	char sendMessage[MAXSTRLEN];

	cout << "1) Start server" << endl;
	cout << "2) Connect to server" << endl;
	cout << "3) Exit" << endl;	
	cin >> nChoice;
	
	if (nChoice == 1)
	{
		ServerSocket server;
		cout << "Starting server..." << endl;
		server.StartHosting(port);

		while (true)
		{
			cout << "\tWaiting..." << endl;
			server.ReciveData(reciveMessage, MAXSTRLEN);
			cout << "Recived: " << reciveMessage << endl;
			server.SendDataMessage();

			if (strcmp(reciveMessage, "end") == 0 || strcmp(sendMessage, "end") == 0)
				break;
		}
	}
	else if (nChoice == 2)
	{
		cout << "Enter an IP address: " << endl;
		cin >> ipAdress;
		ClientSocket client;
		client.ConnectToServer(ipAdress.c_str(), port);
		while (true)
		{
			client.SendDataMessage();
			cout << "\tWaiting" << endl;
			client.ReciveData(reciveMessage, MAXSTRLEN);
			cout << "Recived: " << reciveMessage << endl;
			if (strcmp(reciveMessage, "end") == 0 || strcmp(sendMessage, "end") == 0)
				break;
		}

		client.CloseConnection();
	}
	else if (nChoice == 3)
		return 0;
}

