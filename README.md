# MiniTube  

## Overview  

MiniTube is a WPF application designed to provide a seamless experience for browsing, searching, and viewing videos. With features such as personalized content, search functionality, and a dedicated studio view, MiniTube aims to simplify video interaction while maintaining a user-friendly interface.  

---

## Features  

### Application Structure  

- **Main Entry Point**:  
  - The application starts with the `App.xaml` and `App.xaml.cs` files, setting up the app's lifecycle and resources.  

- **MiniTube App Core**:  
  - The app includes modules for user interaction, video playback, and video management.  

### Core Features  

- **User Authentication**:  
  - Log in to access personalized video recommendations.  

- **Video Browsing**:  
  - Explore a randomized list of videos on the main screen.  

- **Search Functionality**:  
  - Find videos by entering titles or keywords in the search bar.  

- **Video Playback**:  
  - Click on a video to open a dedicated playback screen with essential controls.  

- **Studio View**:  
  - Upload and manage your videos in the studio view.  

---

## Installation  

### Prerequisites  

1. **Database Setup**:  

   - Create the required database tables using the following SQL script:  

   ```sql  
   CREATE TABLE [dbo].[Users] (  
       [User ID] INT IDENTITY(1, 1) NOT NULL,  
       [Username] VARCHAR(255) NOT NULL,  
       [Email] VARCHAR(255) NOT NULL,  
       [Password] VARCHAR(255) NOT NULL,  
       [Role] VARCHAR(50) NOT NULL,  
       PRIMARY KEY CLUSTERED ([User ID] ASC)  
   );  

   CREATE TABLE [dbo].[Videos] (  
       [VideoID] INT IDENTITY(1, 1) NOT NULL,  
       [User ID] INT NOT NULL,  
       [Title] VARCHAR(255) NOT NULL,  
       [Description] TEXT NULL,  
       [Thumbnail] VARBINARY(MAX) NULL,  
       [VideoFile] VARBINARY(MAX) NULL,  
       [Keyword1] VARCHAR(100) NULL,  
       [Keyword2] VARCHAR(100) NULL,  
       [Keyword3] VARCHAR(100) NULL,  
       [UploadDate] DATETIME DEFAULT GETDATE() NULL,  
       [CommentsCount] INT DEFAULT 0 NULL,  
       [LikesCount] INT DEFAULT 0 NULL,  
       PRIMARY KEY CLUSTERED ([VideoID] ASC),  
       FOREIGN KEY ([User ID]) REFERENCES [dbo].[Users] ([User ID])  
   );  

   CREATE TABLE [dbo].[Comments] (  
       [CommentID] INT IDENTITY(1, 1) NOT NULL,  
       [VideoID] INT NOT NULL,  
       [User ID] INT NOT NULL,  
       [CommentText] TEXT NULL,  
       [CommentDate] DATETIME DEFAULT GETDATE() NULL,  
       PRIMARY KEY CLUSTERED ([CommentID] ASC),  
       FOREIGN KEY ([User ID]) REFERENCES [dbo].[Users] ([User ID]),  
       FOREIGN KEY ([VideoID]) REFERENCES [dbo].[Videos] ([VideoID])  
   );  

   CREATE TABLE [dbo].[Likes] (  
       [LikeID] INT IDENTITY(1, 1) NOT NULL,  
       [User ID] INT NOT NULL,  
       [VideoID] INT NOT NULL,  
       [LikedDate] DATETIME DEFAULT GETDATE() NULL,  
       PRIMARY KEY CLUSTERED ([LikeID] ASC),  
       FOREIGN KEY ([User ID]) REFERENCES [dbo].[Users] ([User ID]),  
       FOREIGN KEY ([VideoID]) REFERENCES [dbo].[Videos] ([VideoID])  
   );  
   ```  

2. **Scaffold the Database Context**:  

   - Use the following command to scaffold the Entity Framework Core DbContext:  
     ```bash  
     Scaffold-DbContext 'Server=YOUR_SERVER_NAME;Database=MiniTube;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False' Microsoft.EntityFrameworkCore.SqlServer -OutputDir ModelsEAD -Force  
     ```  
   - Replace `YOUR_SERVER_NAME` with your SQL Server instance (e.g., `DESKTOP-I6UBJ5U\SQLEXPRESS`).  

---

### Steps  

1. **Clone the Repository**:  
    ```bash  
    git clone https://github.com/MalikShujaatAli/MiniTube.git  
    ```  

2. **Open the Solution**:  
   - Launch the project in Visual Studio.  

3. **Restore Dependencies**:  
   - Use NuGet to restore all required packages.  

4. **Build the Solution**:  
   - Ensure the solution builds without errors.  

5. **Run the Application**:  
   - Start the app to explore its functionality.  

---

## Usage  

1. **Launch the Application**:  
   - Open MiniTube and log in using your credentials.  

2. **Browse Video Content**:  
   - Explore the home screen for a curated list of videos.  

3. **Search for Videos**:  
   - Use the search bar to find specific videos by title or keyword.  

4. **Play Videos**:  
   - Select a video to navigate to its playback screen.  

5. **Access the Studio View**:  
   - Upload and manage your videos in the dedicated studio interface.  

---

## Code Structure  

- **View**:  
  - Contains WPF windows and user controls for UI presentation.  

- **Model**:  
  - Defines the data structure and manages interactions with the database context.  

- **ViewModel**:  
  - Acts as the bridge between the view and model, handling data binding and business logic.  

---

## Contributing  

Contributions are welcome! To contribute:  

1. **Fork the Repository**.  
2. **Create a Feature Branch**.  
3. **Commit Your Changes**.  
4. **Push to Your Branch**.  
5. **Create a Pull Request**.  

---

## Acknowledgments  

- Thanks to the contributors and the community for their valuable feedback.  
- Special thanks to the tools and frameworks that made this project possible.  

